using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EnemySOUtility
{
    private static string soFolder = "Assets/Datas/SOs";
    private static string csvPath = "Assets/Resources/DataTables/MonsterTable.csv";

    // 1️⃣ CSV → SO 생성
    [MenuItem("Tools/Generate Enemy SOs")]
    public static void GenerateEnemySOs()
    {
        if (!File.Exists(csvPath))
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {csvPath}");
            return;
        }

        string[] lines = File.ReadAllLines(csvPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 11) continue;

            string monID = values[0];
            string monName = values[1];
            EnemyAttackType atkType = (EnemyAttackType)int.Parse(values[2]);

            string assetPath = $"{soFolder}/{monName}.asset";

            if (AssetDatabase.LoadAssetAtPath<EnemyData>(assetPath) != null)
                continue;

            EnemyData so = ScriptableObject.CreateInstance<EnemyData>();
            so.monID = monID;
            so.monName = (EnemyName)Enum.Parse(typeof(EnemyName), monName);
            so.atkType = atkType;
            so.damage = int.Parse(values[3]);
            so.maxHp = int.Parse(values[4]);
            so.moveSpeed = float.Parse(values[5]);
            so.dropExp = (PickUpType)int.Parse(values[6]);
            so.dropPercent = float.Parse(values[7]);
            so.projectileRange = float.Parse(values[8]);
            so.projectileCooldown = float.Parse(values[9]);
            so.projectileSpeed = float.Parse(values[10]);

            if (!AssetDatabase.IsValidFolder(soFolder))
                AssetDatabase.CreateFolder("Assets/Datas", "SOs");

            AssetDatabase.CreateAsset(so, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Enemy SO 생성 완료!");
    }

    // 2️⃣ SO → CSV 내보내기
    [MenuItem("Tools/Export Enemy SOs to CSV")]
    public static void ExportSOsToCSV()
    {
        if (!Directory.Exists(soFolder))
        {
            Debug.LogError($"SO 폴더를 찾을 수 없습니다: {soFolder}");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:EnemyData", new[] { soFolder });
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("MON_ID,MON_NAME,MON_ATK_TYPE,ATK,MAXHP,MOVE_SPEED,DROP_EXP,DROP_PER,PROJECTILE_RANGE,PROJECTILE_COOLTIME,PROJECTILE_MOVE_SPEED");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyData so = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            if (so == null) continue;

            sb.AppendLine($"{so.monID},{so.monName},{(int)so.atkType},{so.damage},{so.maxHp},{so.moveSpeed},{(int)so.dropExp},{so.dropPercent},{so.projectileRange},{so.projectileCooldown},{so.projectileSpeed}");
        }

        string exportPath = "Assets/Resources/DataTables/MonsterTable_Export.csv";
        File.WriteAllText(exportPath, sb.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"Enemy SO 데이터를 CSV로 저장 완료: {exportPath}");
    }
}
