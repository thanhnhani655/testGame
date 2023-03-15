using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderSpawner : MonoBehaviour
{
    public event System.Action<List<Invader>> InvaderSpawnDone = delegate { };
    [SerializeField]
    private Invader invaderPrefap;

    [SerializeField]
    private Room Gate;

    [SerializeField]
    private List<MonsterData> listInvaderComing;

    [SerializeField]
    private List<Invader> listInvader;
    [SerializeField]
    private GameObject InvaderArea;

    [SerializeField]
    private float timeDelaySpawn = 0.5f;

    private void Start()
    {
        listInvader = new List<Invader>();
        listInvaderComing = new List<MonsterData>();
    }

    public void GetListInvaderComing(List<MonsterData> listDatas)
    {
        listInvaderComing = listDatas;
    }

    public void BeginSpawn()
    {
        StartCoroutine(SpawnForSecond());
    }

    private void Spawn(MonsterData data)
    {
        Invader newInvader = Instantiate<Invader>(invaderPrefap, InvaderArea.transform);
        newInvader.SetData(data);
        newInvader.GetComponent<MobInDungeon>().currentRoom = Gate;
        newInvader.transform.position = Gate.transform.position;
        listInvader.Add(newInvader);
    }

    public void RemoveInvader(Invader invader)
    {
        Destroy(invader.gameObject);
        listInvader.Remove(invader);
    }

    public void RemoveAllInvader()
    {
        foreach (Invader invader in listInvader)
        {
            if (invader.gameObject != null)
                Destroy(invader.gameObject);
        }

        listInvader.Clear();
    }

    private IEnumerator SpawnForSecond()
    {
        while (true)
        {
            if (listInvaderComing.Count == 0)
            {
                InvaderSpawnDone(listInvader);
                break;
            }

            Spawn(listInvaderComing[0]);
            listInvaderComing.RemoveAt(0);

            timeDelaySpawn = Random.Range(0.5f, 1.5f);

            yield return new WaitForSeconds(timeDelaySpawn);
        }
    }

}
