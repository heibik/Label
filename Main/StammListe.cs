using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class StammListe
    {
        private List<Tabelle> _allRecords = null;
        private List<Tabelle> _selectedObjects = new List<Tabelle>();
        private List<Tabelle> _searchResults = new List<Tabelle>();


        public StammListe(List<Tabelle> itemList)
        {
            _allRecords = itemList;

            ChangeAuswahlToDefault();
        }

        public void AddRecord(Tabelle record)
        {
            _allRecords.Add(record);
            _searchResults.Add(record);
        }

        public void SetSelectedIds(System.Collections.IList selectedItems)
        {
            _selectedObjects.Clear();
            if(selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    Tabelle record = (Tabelle)item;

                    _selectedObjects.Add(record);
                }
            }
        }



        public void ChangeAuswahlToDefault()
        {
            _selectedObjects.Clear();
            _searchResults.Clear();

            foreach (var record in _allRecords)
            {
                _searchResults.Add(record);
            }
            
        }

        public void ClearAuswahl()
        {
            _searchResults.Clear();
        }

        public void AddAuswahl(int id)
        {
            foreach (var item in _allRecords)
            {
                if(item.iId == id)
                {
                    if (!_searchResults.Contains(item))
                    {
                        _searchResults.Add(item);
                    }
                }
            }
        }

        public List<Tabelle> GetSearchResults()
        {
            return _searchResults;
        }

        public List<Tabelle> GetAllRecords()
        {
            return _allRecords;
        }

        public void DeleteSelectedRecords()
        {
            List<Tabelle> deletedRecords = new List<Tabelle>();

            foreach (var selectedObject in _selectedObjects)
            {
                foreach (var record in _allRecords)
                {
                    if(record.iId == selectedObject.GetId())
                    {
                        deletedRecords.Add(record);
                    }
                }
            }
            foreach (var record in deletedRecords)
            {
                _allRecords.Remove(record);
                _searchResults.Remove(record);
            }

            _selectedObjects.Clear();
        }

        public List<Tabelle> GetSelectedObjects()
        {
            return _selectedObjects;
        }

    }
}
