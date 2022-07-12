
public class HPbar
{
    private LED_Ctrl led;
    private uint x0, y0, w, h, color;
    private float nHP;
    public float nowHP
    {
        private set{
            nHP = value;
        }
        get{
            return nHP;
        }
    }

    public HPbar(LED_Ctrl _led, uint _x0, uint _y0, uint _w, uint _h, uint _color)
    {
        led = _led;
        setLEDdetail(_x0, _y0, _w, _h, _color);
        //clearBar();
        //printHP(iniHP, true);
        nowHP = 1f;
    }

    public void setLEDdetail(uint _x0, uint _y0, uint _w, uint _h, uint _color)
    {
        x0 = _x0;
        y0 = _y0;
        w = _w;
        h = _h;
        color = _color;
    }

    public void printHP(float hp, bool printSide = false)
    {
        if(printSide)
            clearBar();
        nowHP = hp;
        led.printHPbar(x0, y0, w, h, color, hp, printSide);
    }

    public void clearBar()
    {
        nowHP = 0;
        led.printHPbar(x0, y0, w, h, 0, 0);
    }

}
