using System.IO;
using System.Security.Cryptography;
using TERA_Tweaker.consts;

namespace TERA_Tweaker.classes
{
    public static class FileManager
    {

        public static FileInfo GetS1EngineBackup(string gameDir)
        {
            var dirPath = string.Format("{0}\\{1}\\{2}", gameDir, BaseConsts.CONFIG_DIR, BaseConsts.CONFIG_BACKUP);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var filePath = string.Format("{0}\\{1}", dirPath, BaseConsts.UNTOUCHED_S1ENGINE);

            var result = new FileInfo(filePath);
            return result;
        }

        public static FileInfo GetCurrentS1Engine(string gameDir)
        {
            var path = string.Format("{0}\\{1}\\{2}", gameDir, BaseConsts.CONFIG_DIR, BaseConsts.S1ENGINE);
            return new FileInfo(path);
        }

        public static void RemoveReadOnlyFlagIfSet(string filePath)
        {
            var file = new FileInfo(filePath);
            RemoveReadOnlyFlagIfSet(file);
        }

        public static void RemoveReadOnlyFlagIfSet(FileInfo file)
        {
            if (file.IsReadOnly)
                file.IsReadOnly = false;
        }

        public static void SetReadOnlyFlag(string filePath)
        {
            var file = new FileInfo(filePath);
            SetReadOnlyFlag(file);
        }

        public static void SetReadOnlyFlag(FileInfo file)
        {
            file.IsReadOnly = true;
        }


        public static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            FileStream fs1 = first.OpenRead();
            FileStream fs2 = second.OpenRead();

            byte[] firstHash = MD5.Create().ComputeHash(fs1);
            byte[] secondHash = MD5.Create().ComputeHash(fs2);

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                {
                    //Close both FileStreams and return the result
                    fs1.Close();
                    fs2.Close();
                    return false;
                }
            }

            //Close both FileStreams and return the result
            fs1.Close();
            fs2.Close();
            return true;
        }
    }
}
