// See https://aka.ms/new-console-template for more information
using _202402.Modeling;
var x = int.Parse(args[0]);
var y = int.Parse(args[1]);

Console.WriteLine($"所持金{x}円で、濃厚MAXラーメンを{y}個購入希望ですね。");
var カート = AddingOrder.Empty;
var 所持金 = new JapaneseYen(x);
var 注文可能 = カート.買えますか(所持金, new OrderDetail(new 濃厚MAXラーメン(), new ItemAmount(y)));
if (!注文可能)
{
    Console.WriteLine("申し訳ありませんが、所持金が足りません。");
    return;
}
var ラーメン追加済みカート = カート.注文に追加(new OrderDetail(濃厚MAXラーメン.Instance, new ItemAmount(y)));
if (ラーメン追加済みカート is ErrorOrder error)
{
    Console.WriteLine(error.Message);
    return;
}
カート = ラーメン追加済みカート as AddingOrder ?? throw new InvalidOperationException();
Console.WriteLine($"{濃厚MAXラーメン.Instance.Name}を{y}個カートに追加しました。");
Console.WriteLine($"合計金額は{カート.TotalPrice.Value:N0}円です。");

Console.WriteLine($"残りの所持金で買えるだけ{叉焼スペシャル炒飯.Instance.Name}を購入希望ですね。");
var チャーハンの個数 = 0;
while (true)
{
    if (カート.買えますか(所持金, new OrderDetail(叉焼スペシャル炒飯.Instance, new ItemAmount(チャーハンの個数 + 1))))
    {
        チャーハンの個数++;
    }
    else
    {
        break;
    }
}
if (チャーハンの個数 == 0)
{
    Console.WriteLine("チャーハンが買えません。");
}
else
{
    var チャーハン追加済みカート = カート.注文に追加(new OrderDetail(叉焼スペシャル炒飯.Instance, new ItemAmount(チャーハンの個数)));
    if (チャーハン追加済みカート is ErrorOrder error2)
    {
        Console.WriteLine(error2.Message);
        return;
    }
    カート = チャーハン追加済みカート as AddingOrder ?? throw new InvalidOperationException();
    Console.WriteLine($"{叉焼スペシャル炒飯.Instance.Name}を{チャーハンの個数}個カートに追加しました。");
}
Console.WriteLine("決済を実行します。");
var 決済後 = カート.注文を確定する(所持金);
if (決済後 is ErrorOrder error3)
{
    Console.WriteLine(error3.Message);
    return;
}
var 最終カート = 決済後 as ConfirmedOrder ?? throw new InvalidOperationException();
Console.WriteLine("------決済完了------");
foreach (var item in 最終カート.Items)
{
    Console.WriteLine($"{item.Item.Name} : {item.Amount.Value:N0}点");
}
Console.WriteLine($"合計金額は{最終カート.TotalPrice.Value:N0}円です。");
Console.WriteLine($"受料金は{最終カート.受料金.Value:N0}円です。");
Console.WriteLine($"売り上げは{最終カート.売り上げ.Value:N0}円です。");
Console.WriteLine($"おつりは{最終カート.おつり.Value:N0}円です。");
Console.WriteLine("ありがとうございました。");
