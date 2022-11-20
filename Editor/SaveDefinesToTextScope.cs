using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace Kogane
{
    /// <summary>
    /// ビルドに使用した defines の情報を Resources フォルダのテキストファイルに書き込むエディタ拡張
    /// </summary>
    public sealed class SaveDefinesToTextScope : IDisposable
    {
        //================================================================================
        // 変数(static readonly)
        //================================================================================
        private static readonly string DIRECTORY_NAME = $"Assets/{nameof( SaveDefinesToTextScope )}/Resources";
        private static readonly string FILE_NAME      = "defines.txt";

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SaveDefinesToTextScope
        (
            bool   isReleaseBuild,
            string defines
        )
        {
            // リリースビルドにテキストファイルが含まれないように
            // ビルド開始時に削除しています
            Refresh();

            if ( isReleaseBuild ) return;

            var text = string.Join( "\n", defines.Split( ";" ) );

            Directory.CreateDirectory( DIRECTORY_NAME );
            var path = $"{DIRECTORY_NAME}/{FILE_NAME}";
            File.WriteAllText( path, text, Encoding.UTF8 );
            AssetDatabase.ImportAsset( path );
        }

        /// <summary>
        /// ビルド終了時に呼び出されます
        /// </summary>
        public void Dispose()
        {
            Refresh();
        }

        /// <summary>
        /// 作成したテキストファイルを削除します
        /// </summary>
        private static void Refresh()
        {
            var directoryName = Path.GetDirectoryName( DIRECTORY_NAME );
            if ( !AssetDatabase.IsValidFolder( directoryName ) ) return;
            AssetDatabase.DeleteAsset( directoryName );
        }
    }
}