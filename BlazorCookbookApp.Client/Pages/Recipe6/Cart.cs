namespace BlazorCookbookApp.Client.Pages.Recipe6;

/// <summary>
/// Simple shopping cart.
/// </summary>
/// <param name="onStateHasChanged">The callback to invoke when the cart's
/// state changes.</param>
public class Cart(Action onStateHasChanged)
{
    /// <summary>
    /// Products to buy.
    /// </summary>
    public List<string> Content { get; init; } = [];

    /// <summary>
    /// Total price of the products.
    /// </summary>
    public decimal Value { get; private set; }

    /// <summary>
    /// Number of products in the cart.
    /// </summary>
    public int Volume => Content.Count;

    /// <summary>
    /// Adds a product (a standard ticket thats identified by a tariff)
    /// to the cart.
    /// </summary>
    /// <param name="tariff">The tariff of the product.</param>
    /// <param name="price">The price of the product.</param>
    public void Add(string tariff, decimal price)
    {
        Content.Add(tariff);
        Value += price;
        onStateHasChanged?.Invoke();
    }    
}
