using MyDBEntity.Tables.Users;

namespace BasicLibrary.Users;

/// <summary>
/// 学生服务类
/// </summary>
public class StudentAccountServer
{
    MyDB myDB = new();

    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public string Login(string account, string password)
    {
        Student student = myDB.SetE<Student>().ExistCheck(x => x.Account == account, "学生不存在");

        if (student.Password != password) throw Error.Api("密码错误");

        UserModel authStudent = AuthServer.Default.Login(student.Id);

        myDB.Add(new StudentLogin() { Student = student });
        myDB.SaveChanges("login");

        return authStudent.Token;
    }
}