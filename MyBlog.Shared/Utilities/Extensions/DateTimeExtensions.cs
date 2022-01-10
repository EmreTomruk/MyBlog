using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Utilities.Extensions
{
    public static class DateTimeExtensions //Tum bunlari dosyanin orjinal ve unique path'ini almak icin yaptik. Bunun icin yeni bir GUID tanimlayabilirdik. Ancak bu yontemle saniseye kadar ayirdigimiz icin Unique deger saglamis oluruz...
    {
        public static string FullDateAndTimeStringWithUnderscore(this DateTime dateTime) 
        {
            return $"{dateTime.Millisecond}_{dateTime.Second}_{dateTime.Minute}_{dateTime.Hour}_{dateTime.Day}_{dateTime.Month}_{dateTime.Year}";
        }
    }
}

    // Extension methods olusturabilmek icin yapılması gerekenler;
    //1- static bir sınıf olmalı...
    //2- metod icerisine yazılacak metod da static olmalı...
    //3- hangi sınıfa genisletme metodu eklenecekse, ilk this ile soylenir... 
    //4- ekstra bir parametre gelecekse this den sonra normal şekilde tanımlanır...