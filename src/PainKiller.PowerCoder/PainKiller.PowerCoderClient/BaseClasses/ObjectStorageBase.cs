﻿using PainKiller.CommandPrompt.CoreLib.Modules.StorageModule.Services;
using PainKiller.PowerCoderClient.Contracts;

namespace PainKiller.PowerCoderClient.BaseClasses;

public class ObjectStorageBase<T, TItem> where T : IDataObjects<TItem>, new() where TItem : class, new()
{
    private T _dataObject = StorageService<T>.Service.GetObject();

    public List<TItem> GetItems(bool reload = false)
    {
        if (reload) _dataObject = StorageService<T>.Service.GetObject();
        return _dataObject.Items;
    }
    public void SaveItems(List<TItem> items)
    {
        _dataObject.Items = items;
        _dataObject.LastUpdated = DateTime.Now;
        StorageService<T>.Service.StoreObject(_dataObject);
    }
    public void Insert(TItem item, Func<TItem, bool> match)
    {
        var existing = _dataObject.Items.FirstOrDefault(match);
        if (existing != null) _dataObject.Items.Remove(existing);
        _dataObject.Items.Add(item);
        _dataObject.LastUpdated = DateTime.Now;
        StorageService<T>.Service.StoreObject(_dataObject);
    }
    public bool Remove(Func<TItem, bool> match)
    {
        var existing = _dataObject.Items.FirstOrDefault(match);
        if (existing == null) return false;

        _dataObject.Items.Remove(existing);
        _dataObject.LastUpdated = DateTime.Now;
        StorageService<T>.Service.StoreObject(_dataObject);
        return true;
    }
}