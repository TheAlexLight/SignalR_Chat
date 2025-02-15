﻿using ChatServer.Controllers;
using ChatServer.Helpers;
using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SharedItems.Models.AuthenticationModels;
using SharedItems.Models.StatusModels;
using SharedItems.Enums;
using Newtonsoft.Json;
using SharedItems.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ChatServer.Enums;
using Microsoft.AspNetCore.Hosting;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _dbContext;
        private readonly AccountController _account;
        private readonly RoleController _roleController;
        private readonly AuthorizationHelper _authorizationHelper;
        private readonly GroupsHelper _groupsHelper;
        private readonly UserHelper _userHelper;

        public ChatHub(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _roleController = new RoleController(_roleManager, _userManager);
            _account = new AccountController(_userManager, _dbContext, _roleController);    
            _authorizationHelper = new();
            _groupsHelper = new();
            _userHelper = new UserHelper();
        }

        public async Task SendRegistration(UserRegistrationModel model)
        {
            await _roleController.CreateRoleIfNotExists(Role.User.ToString(), _roleManager);
            await _roleController.CreateRoleIfNotExists(Role.Admin.ToString(), _roleManager);

            AuthorizationHelper helper = new AuthorizationHelper();
            string error = await helper.TryRegistration(model, _userManager, _account);

            bool result = false;    

            if (error == "")
            {
                result = true;

                //List<string> addedRoles = new List<string>()
                //{
                //    "User"
                //};

                //await _roleController.Assign(model.Username, addedRoles);
            }

            await Clients.Caller.SendAsync("ReceiveRegistrationResult", result, error);
        }
            
        public async Task SendLogin(UserLoginModel model)
        {
            bool successfulLogin = await _account.Login(model);

            await Clients.Caller.SendAsync("ReceiveLoginResult", successfulLogin);

            if (successfulLogin)
            {
                UserHandler userHandler = _userHelper.FindUserHandler(Context, model.Username);

                if (userHandler != null)
                {
                    User user = _dbContext.Users
                        .FirstOrDefault(u => u.UserName == model.Username);

                    user.UserModel.UserProfile.IsOnline = true;
                    
                    await _dbContext.SaveChangesAsync();

                    await UpdateChat(userHandler);
                    await SendPublicGroup();

                    if (user.UserModel.UserStatus.BanStatus.IsBanned)
                    {
                        await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveBan", user.UserModel.UserStatus.BanStatus);
                        return;
                    }
                }
            }
        }

        private async Task SendPublicGroup()
        {
            ChatGroupModel group = _dbContext.Groups
                .FirstOrDefault(g => g.Name == ChatType.Public);

            if (group == null)
            {
                group = new ChatGroupModel()
                {
                    Name = ChatType.Public
                };

                await _dbContext.Groups.AddAsync(group);
            }

            group.Users = _dbContext.UserModels.ToList();

            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveCurrentGroup", group);
        }

        public async Task SendReconnection(string username)
        {
            UserHandler userHandler = _userHelper.FindUserHandler(Context, username);

            UserProfileModel user = await _dbContext.UserProfiles
                    .FirstOrDefaultAsync(u => u.Username == username);

            await _dbContext.SaveChangesAsync();

            await UpdateChat(userHandler);

            ChatGroupModel group = _dbContext.Groups
               .FirstOrDefault(g => g.Name == ChatType.Public);

            await Clients.All.SendAsync("ReceiveCurrentGroup", group);
        }

        public async Task SendSwitchChat(ChatType chatType, UserModel currentUser)
        {
            ChatGroupModel group;

            if (chatType == ChatType.Public)
            {
                group = _dbContext.Groups
                      .FirstOrDefault(g => g.Name == ChatType.Public);
            }
            else
            {
                group = new ChatGroupModel();
            }

            UserHandler userHandler = _userHelper.FindUserHandler(Context,currentUser.UserProfile.Username);

            await UpdateChat(userHandler);
            await Clients.Caller.SendAsync("ReceiveCurrentGroup", group);
        }

        public async Task SendUpdatePrivateMessages(UserModel selectedUser, UserModel currentUser)
        {
            ChatGroupModel group = await _dbContext.Groups
                    .FirstOrDefaultAsync(g => g.Name == ChatType.Private
                        && g.Users.Contains(selectedUser)
                        && g.Users.Contains(currentUser));


            group ??= await _groupsHelper.AddUsersToPrivateGroup(_dbContext, currentUser.UserProfile.Username, selectedUser.UserProfile.Username);

            UserHandler userHandler = _userHelper.FindUserHandler(Context, currentUser.UserProfile.Username);

            var timer = new Stopwatch();
            timer.Start();

            await SendCurrentUser(userHandler);

            //B: Run stuff you want timed
            timer.Stop();

            Trace.WriteLine(timer.ElapsedMilliseconds);
            await Clients.Caller.SendAsync("ReceiveCurrentGroup", group);


        }

        public async Task UpdateMessage(MessageModel message)
        {
            MessageModel dbMessage = await _dbContext.Messages
                     .FirstOrDefaultAsync(m => m.Id == message.Id);

            dbMessage.CheckStatus = message.CheckStatus;
            //dbMessage.MessageHeight = message.MessageHeight;

            await _dbContext.SaveChangesAsync();

            //await SendUserList();
        }

        private async Task UpdateChat(UserHandler userHandler)
        {
            await SendCurrentUser(userHandler);
            await SendUserList();
        }

        public async Task SendCurrentUser(UserHandler userHandler)
        {
            User user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.UserName == userHandler.ConnectedUsername);

            string userRole = ((List<string>)await _userManager.GetRolesAsync(user))[0];

            user.UserModel.UserProfile.Role = userRole;

            await _dbContext.SaveChangesAsync();

            await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveCurrentUser", user.UserModel);
        }

        public async Task SendUserList()
        {
            List<UserModel> users = _dbContext.UserModels.ToList();/*.Where(um=>um.UserProfile.Username != userHandler.ConnectedUsername).ToList();*/

            await Clients.All.SendAsync("ReceiveUserList", new ObservableCollection<UserModel>(users));
        }

        public async Task SendMessage(MessageModel message, ChatGroupModel currentGroup, UserModel selectedUser, UserModel currentUser)
        {
            ChatGroupModel group;
            //message.UserModel = currentUser;
                
            if (currentGroup.Name == ChatType.Public)
            {
                group = _dbContext.Groups.FirstOrDefault(g => g.Name == currentGroup.Name);
            }
            else
            {
                group = _dbContext.Groups
                         .FirstOrDefault(g => g.Name == currentGroup.Name
                         && g.Users.Contains(selectedUser)
                         && g.Users.Contains(currentUser));
            }

            if (group != null)
            {
                //message.IsFirstMessage = FirstMessageModel.CheckMessage(message.UserModel.UserProfile.Username);
                group.Messages.Add(message);

                await _dbContext.SaveChangesAsync();

                UserHandler userHandler = _userHelper.FindUserHandler(Context, currentUser.UserProfile.Username);
                
                await UpdateChat(userHandler);

                //await SendConcreteGroup(currentGroup.Name, group);
                await SendPublicGroup();
            }
        }

        public async Task SendDeleteMessage(MessageModel message)
        {
            _dbContext.Messages.Remove(message);

            await _dbContext.SaveChangesAsync();

            UserHandler userHandler = Account.Users.First(a => a.ConnectedIds == Context.ConnectionId);

            await UpdateChat(userHandler);
            await SendPublicGroup();
        }

        public async Task SendUpdateMessage(MessageModel message, ChatGroupModel currentGroup)
        {
            ChatGroupModel foundGroup = await _dbContext.Groups.FirstOrDefaultAsync(g => g == currentGroup);

            if (foundGroup != null)
            {
                MessageModel dbMessage = foundGroup.Messages.FirstOrDefault(m => m.Id == message.Id);

                dbMessage.CheckStatus = message.CheckStatus;
      
                await _dbContext.SaveChangesAsync();

                UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);

                await UpdateChat(userHandler);
                //await SendConcreteGroup(ChatType.Private, foundGroup);
            }
        }

        public async Task SendSubmitUsernameChange(UserModel user, string username, string password)
        {
            User foundUser = await _userManager.FindByNameAsync(user.UserProfile.Username);

            if (foundUser != null)
            {
                if (!_dbContext.Users.Any(up => up.UserName.Equals(username)))
                {
                    if (_authorizationHelper.ValidatePassword(_userManager, foundUser, password) == PasswordVerificationResult.Success)
                    {
                        foundUser.UserName = username;
                        foundUser.UserModel.UserProfile.Username = username;

                        await _userManager.UpdateAsync(foundUser);
                        await _dbContext.SaveChangesAsync();

                        await Clients.Caller.SendAsync("ReceiveUserPropertyChange", true, "Username", string.Empty);

                        UserHandler userHandler = _userHelper.FindUserHandler(Context, username);

                        await UpdateChat(userHandler);
                        await SendPublicGroup();
                    }
                    else
                    {
                        string errorMessage = "Current password doesn't match";
                        await Clients.Caller.SendAsync("ReceiveUserPropertyChange", false, string.Empty, errorMessage);
                    }
                }
                else
                {
                    string errorMessage = "Username is already exist";
                    await Clients.Caller.SendAsync("ReceiveUserPropertyChange", false, string.Empty, errorMessage);
                }
            }
        }

        public async Task SendSubmitEmailChange(UserModel user, string email, string password)
        {
            User foundUser = await _userManager.FindByNameAsync(user.UserProfile.Username);

            if (foundUser != null)
            {
                if (!_dbContext.Users.Any(up => up.Email.Equals(email)))
                {
                    if (_authorizationHelper.ValidatePassword(_userManager, foundUser, password) == PasswordVerificationResult.Success)
                    {
                        foundUser.Email = email;
                        foundUser.UserModel.UserProfile.Email = email;

                        await _userManager.UpdateAsync(foundUser);
                        await _dbContext.SaveChangesAsync();

                        await Clients.Caller.SendAsync("ReceiveUserPropertyChange", true, "Email", string.Empty);

                        UserHandler userHandler = _userHelper.FindUserHandler(Context, user.UserProfile.Username);
                        
                        await UpdateChat(userHandler);
                        await SendPublicGroup();
                    }
                    else
                    {
                        string errorMessage = "Current password doesn't match";
                        await Clients.Caller.SendAsync("ReceiveUserPropertyChange", false, string.Empty, errorMessage);
                    }
                }
                else
                {
                    string errorMessage = "Email is already exist";
                    await Clients.Caller.SendAsync("ReceiveUserPropertyChange", false, string.Empty, errorMessage);
                }
            }
        }

        public async Task SendSubmitPasswordChange(UserModel user, string newPassword, string currentPassword)
        {
            User foundUser = await _userManager.FindByNameAsync(user.UserProfile.Username);

            if (foundUser != null)
            {
                if (_authorizationHelper.ValidatePassword(_userManager, foundUser, currentPassword) == PasswordVerificationResult.Success)
                {
                    foundUser.PasswordHash = _userManager.PasswordHasher.HashPassword(foundUser, newPassword);

                    await _dbContext.SaveChangesAsync();

                    await Clients.Caller.SendAsync("ReceiveUserPropertyChange", true, "Password", string.Empty);

                    UserHandler userHandler = _userHelper.FindUserHandler(Context, user.UserProfile.Username);
                    
                    await UpdateChat(userHandler);
                    await SendPublicGroup();
                }
                else
                {
                    string errorMessage = "Current password doesn't match";
                    await Clients.Caller.SendAsync("ReceiveUserPropertyChange", false, string.Empty, errorMessage);
                }
            }
        }


        private async Task SendConcreteGroup(ChatType groupName, ChatGroupModel group)
        {
            if (groupName == ChatType.Public)
            {
                await Clients.All.SendAsync("ReceiveCurrentGroup", group);
            }
            else
            {
                //await Clients.Caller.SendAsync("ReceiveCurrentGroup", group);

                foreach (UserModel user in group.Users)
                {
                    UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == user.UserProfile.Username);
                    
                    if (userHandler != null)
                    {
                        await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveCurrentGroup", group);
                    }
                }
            }
        }

        public async Task SendChangePhoto(UserModel currentUser, byte[] photo)
        {
            UserModel userModel = _dbContext.UserModels.FirstOrDefault(um => um == currentUser);

            userModel.UserProfile.Image = photo;

            await _dbContext.SaveChangesAsync();

            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == currentUser.UserProfile.Username);

            await UpdateChat(userHandler);
            await SendPublicGroup();
        }

        public async Task SendUserBan(string username, BanStatusModel model)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            User user = _dbContext.Users
                .Include(u => u.UserModel.UserStatus)
                .ThenInclude(userStatus => userStatus.BanStatus)
                .FirstOrDefault(u => u.UserName == username);

            user.UserModel.UserStatus.BanStatus = model;

            await _dbContext.SaveChangesAsync();

            if (model.IsBanned)
            {
                await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveBan", model);
            }

            await UpdateChat(userHandler);
        }

        public async Task SendUserMute(string username, MuteStatusModel model)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            User user = _dbContext.Users
                .FirstOrDefault(u => u.UserName == username);

            user.UserModel.UserStatus.MuteStatus = model;

            await _dbContext.SaveChangesAsync();

            if (model.IsMuted)
            {
                await Clients.Client(userHandler.ConnectedIds).SendAsync("ReceiveMute", model);
            }

            await UpdateChat(userHandler);
        }

        public async Task SendKickUser(string username)
        {
            UserHandler user = Account.Users.FirstOrDefault(u => u.ConnectedUsername == username);

            await Clients.Client(user.ConnectedIds).SendAsync("ReceiveKick");
        }

        public override Task OnConnectedAsync()
        {
            UserHandler userHandler = new()
            {
                ConnectedIds = Context.ConnectionId
            };

            Account.Users.Add(userHandler);

            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            UserHandler userHandler = Account.Users.FirstOrDefault(a => a.ConnectedIds == Context.ConnectionId);
            UserModel userModel = null;

            if (userHandler.ConnectedUsername != null)
            {
                 userModel = await _dbContext.UserModels
                     .FirstOrDefaultAsync(um => um.UserProfile.Username == userHandler.ConnectedUsername);

                userModel.UserProfile.IsOnline = false;
                await _dbContext.SaveChangesAsync();
            }

            Account.Users.Remove(userHandler);

            await UpdateChat(userHandler);
            await Clients.All.SendAsync("ReceiveCurrentGroup", userModel.Groups.FirstOrDefault(g=>g.Name==ChatType.Public));

            return base.OnDisconnectedAsync(exception);
        }
    }
}
