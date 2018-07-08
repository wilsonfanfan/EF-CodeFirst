using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace X.Common
{
    /// <summary>
    /// 验证身份证后，返回的信息，是否正确身份证，出生年月日，性别
    /// </summary>
    [Serializable]
    public class ValidataCardNumberResult
    {
        /// <summary>
        /// 验证是否通过
        /// </summary>
        public bool IsSuccess { set; get; }

        /// <summary>
        /// 性别(是否为男性)
        /// </summary>
        public bool Sex { set; get; }

        /// <summary>
        /// 生日 格式（年-月-日）
        /// </summary>
        public String BirthDate { set; get; }
    }
    /// <summary>
    /// 验证身份证号码
    /// </summary>
    public class ValidataCardNumber
    {
        #region 验证身份证号码

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="value">返回是否通过，出生（年-月-日），性别</param>
        /// <returns></returns>
        public static ValidataCardNumberResult CheckIdentityCardNumber(string value)
        {
            var result = new ValidataCardNumberResult();
            var powers = new String[] { "7", "9", "10", "5", "8", "4", "2", "1", "6", "3", "7", "9", "10", "5", "8", "4", "2" };
            var parityBit = new String[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };

            if (String.IsNullOrEmpty(value)) return result;

            if (!(value.Length == 15 || value.Length == 18)) return result;

            if (value.Length == 15)
            {
                String _id = value;

                //15位的，全部都数字表示
                Match matchNumber = Regex.Match(value, @"^\d{15}$");
                if (!matchNumber.Success) return result;

                var year = _id.Substring(6, 2);
                var month = _id.Substring(8, 2);
                var day = _id.Substring(10, 2);
                var sexBit = _id.Substring(14);
                //校验年份位
                // ^\d{2}$
                Match matchYear = Regex.Match(year, @"^\d{2}$");
                if (!matchYear.Success) return result;
                //校验月份
                Match matchMonth = Regex.Match(month, @"^0[1-9]|1[0-2]$");
                if (!matchMonth.Success) return result;
                //校验日
                Match matchDay = Regex.Match(day, @"^[0-2][1-9]|3[0-1]|10|20$");
                if (!matchDay.Success) return result;
                //设置性别
                result.Sex = sexBit.ToInt() % 2 != 0;

                String strDate = String.Format("19{0}-{1}-{2}", year, month, day);
                DateTime birthDate = DateTime.Now;
                if (DateTime.TryParse(strDate, out birthDate))
                {
                    if (("19" + year).ToInt() != birthDate.Year || month.ToInt() != birthDate.Month ||
                        day.ToInt() != birthDate.Day)
                    {
                        return result;
                    }
                    result.BirthDate = strDate;
                    result.IsSuccess = true;
                }
                return result;
            }
            else if (value.Length == 18)
            {//_valid = validId18(_id);
                String _id = value;
                var _num = _id.Substring(0, 17);
                //18位的，前17位是全部都数字表示
                Match matchNumber = Regex.Match(_num, @"^\d{17}$");
                if (!matchNumber.Success) return result;

                var _parityBit = _id.Substring(17);
                var _power = 0;

                result.Sex = true;
                for (var i = 0; i < 17; i++)
                {
                    //校验每一位的合法性
                    //加权
                    _power += _num[i].ToString().ToInt() * powers[i].ToInt();
                    //设置性别
                    if (i == 16 && _num[i].ToString().ToInt() % 2 == 0)
                    {
                        result.Sex = false;
                    }
                }
                var year = _id.Substring(6, 4);
                var month = _id.Substring(10, 2);
                var day = _id.Substring(12, 2);

                //校验年份位
                Match matchYear = Regex.Match(year, @"^\d{4}$");
                if (!matchYear.Success) return result;
                //校验月份
                Match matchMonth = Regex.Match(month, @"^0[1-9]|1[0-2]$");
                if (!matchMonth.Success) return result;
                //校验日
                Match matchDay = Regex.Match(day, @"^[0-2][0-9]|3[0-1]$");
                if (!matchDay.Success) return result;
                //获取日期
                String strDate = String.Format("{0}-{1}-{2}", year, month, day);
                DateTime birthDate = DateTime.Now;
                if (DateTime.TryParse(strDate, out birthDate))
                {
                    if (year.ToInt() != birthDate.Year || month.ToInt() != birthDate.Month ||
                        day.ToInt() != birthDate.Day)
                    {
                        return result;
                    }
                    result.BirthDate = strDate;
                }
                //取模
                var mod = _power % 11;
                if (parityBit[mod] == _parityBit)
                {
                    result.IsSuccess = true;
                    return result;
                }
                return result;
            }
            return result;
        }
        #endregion
    }
}
