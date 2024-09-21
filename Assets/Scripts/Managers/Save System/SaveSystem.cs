using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

public class SaveSystem
{
    public static void SaveProgress()
    {
        SaveData.Instance.HashOfSaveData = HashGenerator(SaveObjectAsJSON());
        File.WriteAllText(GetSavePath(), JsonUtility.ToJson(SaveData.Instance, true));
    }//SaveProgress() end

    private static string SaveObjectAsJSON() => JsonUtility.ToJson(SaveData.Instance.CreateSaveObject(), true);

    public static void LoadProgress()
    {
        if(File.Exists(GetSavePath())) 
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(GetSavePath()), SaveData.Instance);
            if(!Application.isEditor) // This is for checking Tampering of Save File outside the Edtior Only
            {
                if((HashGenerator(SaveObjectAsJSON()) != SaveData.Instance.HashOfSaveData))
                {
                    SaveData.Instance.Reset();
                    DeleteProgress();
                    SaveProgress();
                }//if end
            }//if end
        }//if end
        else     
            SaveProgress();
    }//LoadProgress() end

    private static string HashGenerator(string SaveContent)
    {
        SHA256Managed Crypt = new SHA256Managed();
        string Hash = string.Empty;
        byte[] Crypto = Crypt.ComputeHash(Encoding.UTF8.GetBytes(SaveContent), 0, Encoding.UTF8.GetByteCount(SaveContent));
        foreach (byte Bit in Crypto)
        {
            Hash += Bit.ToString("x2");
        }//loop end
        return Hash;
    }//HashGenerator() end

    private static void DeleteProgress()
    {
        if(File.Exists(GetSavePath()))
            File.Delete(GetSavePath());
    }//DeleteProgress() end

    private static string GetSavePath() => Path.Combine(Application.persistentDataPath, "SavedGame.json");

//Editor only functions
#if UNITY_EDITOR

    //For Opening SaveData file => Shortcut key (Ctrl or Cmd) + Shift + j
    [UnityEditor.MenuItem("Utils/Save System/Open Save File %#j")]
    private static void OpenSaveFile()
    {
        if(File.Exists(GetSavePath()))
            UnityEditor.EditorUtility.RevealInFinder(GetSavePath());
        else
            UnityEditor.EditorUtility.RevealInFinder(Application.persistentDataPath);
    }//OpenSaveFile() end

    //For Restting Save Data File => Shortcut key (Ctrl or Cmd) + Shift + r
    [UnityEditor.MenuItem("Utils/Save System/Reset Save Data %#r")]
    private static void ResetSaveData()
    {
        if(UnityEditor.EditorUtility.DisplayDialog("Save Data", "Do you want to Reset all Save Data ?", "Yes", "No"))
        {
            UnityEditor.EditorUtility.DisplayDialog("Save Data", "Save Data Reset Succesful!", "Okay");
            DeleteProgress();
        }//if end
    }//ResetSaveData() end

#endif

}//class end