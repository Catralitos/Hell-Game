using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    public class SignalAngels : Task
    {
        public Angel SignalerAngel { get; set; }
        public GameObject BackupTarget { get; set; }

        public SignalAngels(Angel character, GameObject backupTarget)
        {
            this.SignalerAngel = character;
            this.BackupTarget = backupTarget;
        }

        public override Result Run()
        {
            Vector3 BeaconPosition = new Vector3(SignalerAngel.transform.position.x,
                                             SignalerAngel.transform.position.y,
                                             SignalerAngel.transform.position.z);

            Debug.Log("Character found player! Location = " + BeaconPosition.x + "|" + BeaconPosition.y + "|" + BeaconPosition.z);
            return Result.Success;

        }
    }

}