using System;
using System.Collections.Generic;
using System.Text;

namespace NameCaseLib.Core
{
    /// <summary>
    /// Класс содержит функции для работы со строками, которые используются в NCLNameCaseLib
    /// @author Андрей Чайка <bymer3@gmail.com>
    /// @version 0.0.1
    /// </summary>
    static class NCLStr
    {
        /// <summary>
        /// Проверяет находится ли строка в нижнем регистре
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>true если в нижнем регистре и false если нет</returns>
        public static bool isLowerCase(String str)
        {
            if (str == str.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
