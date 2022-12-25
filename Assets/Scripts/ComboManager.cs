public class ComboManager
{

    private ComboManager()
    {
    }

    public void update()
    {
    }
    private static ComboManager i = new ComboManager();
    public static ComboManager I() { return i; }
}