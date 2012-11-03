using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace TickTick.Data
{
    public partial class Level
    {
        /// <summary>
        /// Reads all the level unlocks
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The level</returns>
        public static LevelStatus[] ReadLevelUnlocks(String path)
        {
            List<LevelStatus> result = new List<LevelStatus>();

            // Opens the storage device
            StorageDevice sd =  StorageDevice.EndShowSelector(StorageDevice.BeginShowSelector(null, null));
            StorageContainer sc = sd.EndOpenContainer(sd.BeginOpenContainer("TickTick", (a) => { }, null)); // TODO clean this up
            
            // Read the file from SavedGames/path or if not exists from the default path in Content/ directory
            // You want this because without security you are not allowed to save outside of the APPDATA or
            // my documents folder. The storage device is located inside the my documents folder, so it will
            // never get you into trouble.
            using (Stream stream = (sc.FileExists(path) ? sc.OpenFile(path, FileMode.Open) : TitleContainer.OpenStream("Content/" + path)))
            {
                using (StreamReader reader = new StreamReader(stream)) {

                    Int32 id = 1;
                    String line = String.Empty;
                    while ((line = reader.ReadLine()) != null)
                    { 
                        var splitted_line = line.Split(',');
                        if (splitted_line.Length > 1)
                            result.Add(new LevelStatus(id++, 
                                Boolean.Parse(splitted_line[0]),
                                Boolean.Parse(splitted_line[1])
                                )
                            );
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="levelSolved"></param>
        public static void SaveLevelUnlocks(String path, Int32 levelSolved)
        {
            var unlocks = ReadLevelUnlocks(path);

            // Opens the storage device
            StorageDevice sd = StorageDevice.EndShowSelector(StorageDevice.BeginShowSelector(null, null));
            StorageContainer sc = sd.EndOpenContainer(sd.BeginOpenContainer("TickTick", (a) => { }, null));

            using (Stream stream = sc.OpenFile(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {

                    Int32 id = 1;
                    foreach (var unlock in unlocks) {

                        var line = String.Format("{0},{1}", 
                            id == levelSolved + 5 || unlock.Unlocked, 
                            id == levelSolved || unlock.Solved
                        );
                        writer.WriteLine(line);
                        id++;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public struct LevelStatus
        {
            public Int32 Id;
            public Boolean Unlocked;
            public Boolean Solved;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="unlocked"></param>
            /// <param name="solved"></param>
            public LevelStatus(Int32 id, Boolean unlocked, Boolean solved)
            {
                this.Id = id;
                this.Solved = solved;
                this.Unlocked = unlocked;
            }

        }
    }
}
