using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraShredder : MonoBehaviour
{
    [SerializeField] bool hasMeteorSpawnerScriptInScene = false;
    [SerializeField] GameObject meteorSpawner;
    MeteorSpawnerScript meteorSpawnerScript;

    void Start()
    {
        if (hasMeteorSpawnerScriptInScene == true)
        {
            meteorSpawnerScript = meteorSpawner.GetComponent<MeteorSpawnerScript>();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (hasMeteorSpawnerScriptInScene)
        {

            var otherGameObjectName = other.gameObject.name;
            var otherGameObjectTag = other.gameObject.tag;
            var otherGameObject = other.gameObject;

            //otherGameObject.GetComponent<NewMeteorScript>().Stop_This_meteor();

            switch (otherGameObjectName)
            {
                case "Toby A Smith":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Jackie B Landon":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Jackie C Price":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Jackie D White":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Robert E Jacobs":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Robert F morris":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Robert G Johnson":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "tina":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "jacqueline":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "herbert":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "matty":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "dennis":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Meteor Object List");
                    break;
                case "Diego Santos":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Yanna Rosales":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Yanna Jimenez":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Yanna Taguptup":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Hyacynth Ramirez":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Hyacynth Lopez":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "Hyacynth Villareal":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "totoy":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "opay":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "bantay":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "bulabog":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                case "cordapia":
                    meteorSpawnerScript.Add_back_to_list(other.gameObject, "Spawn Frozen Meteor Object List");
                    break;
                default:
                    switch (otherGameObjectTag)
                    {
                        case "hugeMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "hugeFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "bigMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "bigFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_bigMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_bigFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "madeMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "medFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_madeMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_madeFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "tinyMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "tinyFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_tinyMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        case "sfd_tinyFrozenMeteor":
                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(other.gameObject);
                            break;
                        default:
                            Destroy(other.gameObject);
                            break;
                    }
                    break;

            }
        }
        else
        {
            Destroy(other.gameObject);
        }

    }
}
