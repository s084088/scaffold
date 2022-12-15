using System;
using System.Collections.Generic;
using System.Linq;
using Auth.Models;
using Util;

namespace Auth;

/// <summary>
/// 用户验证类
/// </summary>
public class AuthServer
{
    #region 私有字段&属性
    private readonly double outTime;//超时时间(分钟)
    private readonly List<UserModel> cache; //用户缓存

    /// <summary>
    /// 重新设置过期时间
    /// </summary>
    /// <returns></returns>
    private DateTime NewExpirationTime => DateTime.Now.AddSeconds(outTime);
    #endregion

    #region 对外接口&构造函数

    /// <summary>
    /// 初始化,构造函数
    /// </summary>
    /// <param name="timeOut">过期时间 秒</param>
    public AuthServer(double timeOut = 7200)
    {
        cache = new();
        outTime = timeOut;//超时时间(分钟)
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="userId"></param>
    /// <returns></returns>
    public UserModel Login(string userId)
    {
        LoginOutById(userId);

        UserModel user = new()
        {
            Token = GuidHelper.GenerateKey(),
            LastTime = NewExpirationTime,
            UserId = userId
        };

        cache.Add(user);

        return user;
    }

    /// <summary>
    /// 校验Token
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ValidationResult Check(string token, out UserModel user)
    {
        user = GetUserByToken(token);

        if (user == null) return ValidationResult.Invalid;                      //Token找不到

        if (user.LastTime < DateTime.Now) return ValidationResult.Expired;      //Token过期

        return ValidationResult.Success;
    }

    /// <summary>
    /// 根据ID登出
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool LoginOutById(string userId)
    {
        UserModel user = GetUserById(userId);

        if (user == null) return false;

        cache.Remove(user);
        return true;
    }

    /// <summary>
    /// 根据Token登出
    /// </summary>
    /// <param name="token">用户Token</param>
    /// <returns></returns>
    public bool LogoutByToken(string token)
    {
        UserModel user = GetUserByToken(token);

        if (user == null) return false;

        cache.Remove(user);
        return true;
    }

    /// <summary>
    /// 取用户
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public UserModel GetUserById(string userId) => cache.FirstOrDefault(x => x.UserId == userId);

    /// <summary>
    /// 取用户
    /// </summary>
    /// <param name="token">Token</param>
    /// <returns></returns>
    public UserModel GetUserByToken(string token) => cache.FirstOrDefault(x => x.Token == token);
    #endregion

    /// <summary>
    /// 默认实例
    /// </summary>
    public static AuthServer Default => _auth ??= new AuthServer();
    private static AuthServer _auth;
}
