
public interface IState
{
    public void OnEnter();
    public void Start();
    public void OnExit();
    public void Update();
    public void FixedUpdate();
    public void LateUpdate();
}
