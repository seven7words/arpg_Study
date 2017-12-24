using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public delegate void  WarningResult();
public class WarningModel
{
    public WarningResult result;
    public string value;
    public float delay;
    public WarningModel(string value,WarningResult result=null,float delay = -1){
        this.value = value;
        this.result = result;
        this.delay = delay;

    }
}
