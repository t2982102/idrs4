using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.PoJo
{
    public class Result
    {


        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static Result PassResult()
        {
            Result aa = new Result();
            aa.ErrorCode = 1;
            aa.ErrorMessage = "获取成功/操作成功！";
            return aa;
        }
        public static Result ErrorResult(int ErrorCode)
        {
            Result aa = new Result();
            switch (ErrorCode)
            {
                case -1:
                    aa.ErrorCode = -1;
                    aa.ErrorMessage = "获取失败！";
                    break;
                case -2:
                    aa.ErrorCode = -2;
                    aa.ErrorMessage = "填写参数不全";
                    break;
                case -3:
                    aa.ErrorCode = -3;
                    aa.ErrorMessage = "查询不到该用户（道路货物运输）";
                    break;
                case -4:
                    aa.ErrorCode = -4;
                    aa.ErrorMessage = "更新诚信考核日期失败！";
                    break;
                case -5:
                    aa.ErrorCode = -5;
                    aa.ErrorMessage = "新增用户失败！字段匹配不对,有可能是必填项没填！";
                    break;
                case -6:
                    aa.ErrorCode = -6;
                    aa.ErrorMessage = "该用户已存在不可重复新增！";
                    break;
                case -7:
                    aa.ErrorCode = -7;
                    aa.ErrorMessage = "修改用户信息失败！";
                    break;
                case -8:
                    aa.ErrorCode = -8;
                    aa.ErrorMessage = "该用户诚信考核记录异常，请查询修改后再次上传成绩！";
                    break;
                case -9:
                    aa.ErrorCode = -9;
                    aa.ErrorMessage = "访问不到LBS接口地址";
                    break;
                case -10:
                    aa.ErrorCode = -10;
                    aa.ErrorMessage = "没有发送电话号码";
                    break;
                case -11:
                    aa.ErrorCode = -11;
                    aa.ErrorMessage = "获取LBS数据失败";
                    break;
                case -12:
                    aa.ErrorCode = -12;
                    aa.ErrorMessage = "LBS移动端报错，请用调用type=test查看具体错误信息并与移动联系";
                    break;
                case -13:
                    aa.ErrorCode = -13;
                    aa.ErrorMessage = "开启apikey验证,验证未通过！";
                    break;
                case -14:
                    aa.ErrorCode = -14;
                    aa.ErrorMessage = "开启ip白名单验证,你的ip不在名单中";
                    break;
                case -15:
                    aa.ErrorCode = -15;
                    aa.ErrorMessage = "获取BOSS数据失败！";
                    break;
                case -16:
                    aa.ErrorCode = -16;
                    aa.ErrorMessage = "没有比你提交的数据日期大的文件！";
                    break;
                case -17:
                    aa.ErrorCode = -17;
                    aa.ErrorMessage = "与移动连接出错";
                    break;
                case -18:
                    aa.ErrorCode = -18;
                    aa.ErrorMessage = "短信发送不成功！返回错误代码：";
                    break;
                case -19:
                    aa.ErrorCode = -19;
                    aa.ErrorMessage = "移动短信发送失败,请查看相关报错内容";
                    break;
                case -20:
                    aa.ErrorCode = -20;
                    aa.ErrorMessage = "请填写完整信息！";
                    break;
                case -21:
                    aa.ErrorCode = -21;
                    aa.ErrorMessage = "用户名不存在！";
                    break;
                case -22:
                    aa.ErrorCode = -22;
                    aa.ErrorMessage = "用户名或密码不正确！";
                    break;
                case -23:
                    aa.ErrorCode = -23;
                    aa.ErrorMessage = "验证码错误！";
                    break;
                case -24:
                    aa.ErrorCode = -24;
                    aa.ErrorMessage = "修改密码失败：";
                    break;
                case -25:
                    aa.ErrorCode = -25;
                    aa.ErrorMessage = "删除用户失败：";
                    break;
                case -26:
                    aa.ErrorCode = -26;
                    aa.ErrorMessage = "原始密码不正确！";
                    break;
                case -27:
                    aa.ErrorCode = -27;
                    aa.ErrorMessage = "修改室温控制失败:";
                    break;
                case -28:
                    aa.ErrorCode = -28;
                    aa.ErrorMessage = "登陆权限Session出错:";
                    break;
                case -29:
                    aa.ErrorCode = -29;
                    aa.ErrorMessage = "查看室温负责人失败：";
                    break;
                case -30:
                    aa.ErrorCode = -30;
                    aa.ErrorMessage = "修改室温负责人数据失败：";
                    break;
                case -31:
                    aa.ErrorCode = -31;
                    aa.ErrorMessage = "修改室温考核失败：";
                    break;
                case -32:
                    aa.ErrorCode = -32;
                    aa.ErrorMessage = "访问接口失败，请联系管理员！";
                    break;
                case -33:
                    aa.ErrorCode = -33;
                    aa.ErrorMessage = "格式转换错误请查看输入参数：";
                    break;
                case -34:
                    aa.ErrorCode = -34;
                    aa.ErrorMessage = "上传文件格式不对！请不要乱上传其他格式文件！";
                    break;
                case -35:
                    aa.ErrorCode = -35;
                    aa.ErrorMessage = "文件保存失败，请查看服务器设置或上传路径磁盘是否充足！";
                    break;
                case -36:
                    aa.ErrorCode = -36;
                    aa.ErrorMessage = "未找到上传后的文件,请核对上传文件！";
                    break;
                case -37:
                    aa.ErrorCode = -37;
                    aa.ErrorMessage = "导入出错：";
                    break;
                case -38:
                    aa.ErrorCode = -38;
                    aa.ErrorMessage = "导入数据库出错：";
                    break;
                case -39:
                    aa.ErrorCode = -39;
                    aa.ErrorMessage = "查无此微信账号绑定，请重新登录！";
                    break;
               
                case -101:
                    aa.ErrorCode = -101;
                    aa.ErrorMessage = "安全验证未设置,请查看配置文件";
                    break;
                default:
                    aa.ErrorCode = -100;
                    aa.ErrorMessage = "其他错误(无该type返回)";
                    break;
            }
            return aa;

        }

    }
}
