using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
  private readonly IList<Item> _items;

  /// <summary>
  /// A dictionary of item updaters for all item types
  /// </summary>
  private readonly IDictionary<string, IItemUpdater> _updaters;

  public GildedRose(IList<Item> items)
  {
    _items = items;
    _updaters = new Dictionary<string, IItemUpdater>
        {
            {"Normal", new NormalItemUpdater()},
            {"Aged Brie", new AgedBrieUpdater()},
            {"Sulfuras, Hand of Ragnaros", new SulfurasUpdater()},
            {"Backstage passes to a TAFKAL80ETC concert", new BackstagePassesUpdater()},
            {"Conjured", new ConjuredItemUpdater()}
        };
  }

  public void UpdateQuality()
  {
    foreach (var item in _items)
    {
      // Using the name to get the updater
      var updater = _updaters[item.Name];
      updater.UpdateQuality(item);
    }
  }

  // public void UpdateQuality()
  // {
  //     for (var i = 0; i < _items.Count; i++)
  //     {
  //         if (_items[i].Name != "Aged Brie" && _items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
  //         {
  //             if (_items[i].Quality > 0)
  //             {
  //                 if (_items[i].Name != "Sulfuras, Hand of Ragnaros")
  //                 {
  //                     _items[i].Quality = _items[i].Quality - 1;
  //                 }
  //             }
  //         }
  //         else
  //         {
  //             if (_items[i].Quality < 50)
  //             {
  //                 _items[i].Quality = _items[i].Quality + 1;

  //                 if (_items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
  //                 {
  //                     if (_items[i].SellIn < 11)
  //                     {
  //                         if (_items[i].Quality < 50)
  //                         {
  //                             _items[i].Quality = _items[i].Quality + 1;
  //                         }
  //                     }

  //                     if (_items[i].SellIn < 6)
  //                     {
  //                         if (_items[i].Quality < 50)
  //                         {
  //                             _items[i].Quality = _items[i].Quality + 1;
  //                         }
  //                     }
  //                 }
  //             }
  //         }

  //         if (_items[i].Name != "Sulfuras, Hand of Ragnaros")
  //         {
  //             _items[i].SellIn = _items[i].SellIn - 1;
  //         }

  //         if (_items[i].SellIn < 0)
  //         {
  //             if (_items[i].Name != "Aged Brie")
  //             {
  //                 if (_items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
  //                 {
  //                     if (_items[i].Quality > 0)
  //                     {
  //                         if (_items[i].Name != "Sulfuras, Hand of Ragnaros")
  //                         {
  //                             _items[i].Quality = _items[i].Quality - 1;
  //                         }
  //                     }
  //                 }
  //                 else
  //                 {
  //                     _items[i].Quality = _items[i].Quality - _items[i].Quality;
  //                 }
  //             }
  //             else
  //             {
  //                 if (_items[i].Quality < 50)
  //                 {
  //                     _items[i].Quality = _items[i].Quality + 1;
  //                 }
  //             }
  //         }
  //     }
  // }
}
