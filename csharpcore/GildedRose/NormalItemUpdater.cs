using GildedRoseKata;

public class NormalItemUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    DecreaseSellIn(item);
    DecreaseQuality(item);

    if (item.SellIn < 0)
    {
      DecreaseQuality(item); // Once sell by date has passed, Quality degrades twice as fast
    }
  }

  private void DecreaseSellIn(Item item)
  {
    item.SellIn -= 1;
  }

  private void DecreaseQuality(Item item)
  {
    if (item.Quality > 0)
    {
      item.Quality -= 1;
    }
  }
}