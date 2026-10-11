using UnityEngine;

public static class Options
{
    private static int _frame;
    public static int Frame
    {
        get { return _frame; }
        set
        {
            if (_frame != value)
            {
                _frame = value;
                Application.targetFrameRate = _frame;
                Debug.Log($"Set Frame: {value}");
            }
        }
    }

    public static void initialize()
    {
        Frame = 60;
    }
}