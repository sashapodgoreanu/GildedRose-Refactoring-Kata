using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void Foo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("fixme"));
    }


    // I will Start by writing tests for the existing behaviors before introducing "Conjured" items. 
    // This ensures that when refactor, i do not alter the intended functionality.
    

    /// <summary>
    /// This test the behavior of a normal item: The quality degrades by one and the sellIn decreases by one
    /// </summary>
    [Test]
    public void NormalItem_QualityDecreasesByOne()
    {
        var items = new List<Item> { new Item { Name = "Normal", SellIn = 10, Quality = 20 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(9));
        Assert.That(items[0].Quality, Is.EqualTo(19));
    }

    /// <summary>
    /// This test the behavior of a Aged Brie: increases in quality as it ages
    /// </summary>
    [Test]
    public void AgedBrie_IncreasesInQualityOverTime()
    {
        var items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(1));
    }

    /// <summary>
    /// This test the behavior of a Sulfuras: never decreases in quality or sell-in
    /// </summary>

    [Test]
    public void Sulfuras_DoesNotChange()
    {
        var items = new List<Item> { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(0));
        Assert.That(items[0].Quality, Is.EqualTo(80));
    }
}