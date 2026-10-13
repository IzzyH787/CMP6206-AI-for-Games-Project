using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCommands : MonoBehaviour
{
    static private InputAction sitAction;
    static private InputAction walkUpAction;
    static private InputAction comeByAction;
    static private InputAction awayAction;

    // Runs when enterring run time
    [RuntimeInitializeOnLoadMethod]
    static void FindActions()
    {
        sitAction = InputSystem.actions.FindAction("Sit");
        sitAction?.Enable(); // might return null if name ever changed
        walkUpAction = InputSystem.actions.FindAction("Walk Up");
        walkUpAction?.Enable(); // might return null if name ever changed
        comeByAction = InputSystem.actions.FindAction("Come By");
        comeByAction?.Enable(); // might return null if name ever changed
        awayAction = InputSystem.actions.FindAction("Away");
        awayAction?.Enable(); // might return null if name ever changed
    }
    public static bool SitCommandGiven
    {
        get
        {
            return sitAction.WasPressedThisFrame();
        }
    }
    public static bool WalkUpCommandGiven
    {
        get
        {
            return walkUpAction.WasPressedThisFrame();
        }
    }
    public static bool ComeByCommandGiven
    {
        get
        {
            return comeByAction.WasPressedThisFrame();
        }
    }
    public static bool AwayCommandGiven
    {
        get
        {
            return awayAction.WasPressedThisFrame();
        }
    }

}
