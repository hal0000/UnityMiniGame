using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public PlayerModel PlayerData;

    public virtual void Awake()
    {
        PlayerData = ServiceLocator.Get<PlayerModel>();
    }

    public virtual void OnEnable()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void OnDisable()
    {
    }

    public virtual void OnDestroy()
    {
    }
}