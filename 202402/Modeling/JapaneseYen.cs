using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
namespace _202402.Modeling;

public record JapaneseYen(
    [property: Range(0, 99999999)]
    int Value);
public interface IItem
{
    public string Name { get; }
    public JapaneseYen Price { get; }
}
public record 濃厚MAXラーメン : IItem
{
    public static 濃厚MAXラーメン Instance => new();
    public string Name => "濃厚 MAX ラーメン";
    public JapaneseYen Price => new(1080);
}
public record 叉焼スペシャル炒飯 : IItem
{
    public static 叉焼スペシャル炒飯 Instance => new();
    public string Name => "叉焼スペシャル炒飯";
    public JapaneseYen Price => new(980);
}
public interface IOrder
{
    public ImmutableList<OrderDetail> Items { get; }
    public JapaneseYen TotalPrice { get; }
}
public record ItemAmount(
    [property: Range(1, 100)]
    int Value);
public record OrderDetail(IItem Item, ItemAmount Amount);
public record AddingOrder(ImmutableList<OrderDetail> Items) : IOrder
{
    public static AddingOrder Empty => new(ImmutableList<OrderDetail>.Empty);
    public JapaneseYen TotalPrice => new(Items.Select(x => x.Item.Price.Value * x.Amount.Value).Sum());
    public bool 買えますか(JapaneseYen 所持金, OrderDetail 注文) => TotalPrice.Value + 注文.Item.Price.Value * 注文.Amount.Value <= 所持金.Value;
    public IOrder 注文に追加(OrderDetail 注文) => Items.Any(i => i.Item.Equals(注文.Item)) ? new ErrorOrder(Items, "すでに同じ商品が入っています", new JapaneseYen(0))
        : new AddingOrder(Items.Add(注文));
    public IOrder 注文を確定する(JapaneseYen 所持金) => TotalPrice.Value <= 所持金.Value ? new ConfirmedOrder(
            Items,
            所持金,
            new JapaneseYen(TotalPrice.Value),
            new JapaneseYen(所持金.Value - TotalPrice.Value))
        : new ErrorOrder(Items, "所持金が足りません", 所持金);
}
public record ConfirmedOrder(ImmutableList<OrderDetail> Items, JapaneseYen 受料金, JapaneseYen 売り上げ, JapaneseYen おつり) : IOrder
{
    public JapaneseYen TotalPrice => new(Items.Select(x => x.Item.Price.Value * x.Amount.Value).Sum());
}
public record ErrorOrder(ImmutableList<OrderDetail> Items, string Message, JapaneseYen おつり) : IOrder
{
    public JapaneseYen TotalPrice => new(Items.Select(x => x.Item.Price.Value * x.Amount.Value).Sum());
}
