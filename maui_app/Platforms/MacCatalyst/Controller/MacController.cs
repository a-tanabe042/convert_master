using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maui_app.Platforms.MacCatalyst.Controller
{
    public class MacController
    {
        /// <summary>
        /// ファイル取込み
        /// </summary>
        /// <param name="filename">取込みファイル名</param>
        /// <returns>処理結果（true: 成功, false: 失敗）</returns>
        public bool ImportInputFile(string filename)
        {
            Debug.WriteLine(filename);
            return true;
        }
    }
}
