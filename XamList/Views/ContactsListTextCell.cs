﻿using System;
using System.Linq;
using Xamarin.Forms;

namespace XamList
{
    public class ContactsListTextCell : TextCell
    {
        #region Constant Fields
        readonly MenuItem _deleteAction;
        #endregion

        #region Constructors
        public ContactsListTextCell()
        {
            TextColor = Color.FromHex("1B2A38");
            DetailColor = Color.FromHex("2B3E50");

            _deleteAction = new MenuItem
            {
                Text = "Delete",
                IsDestructive = true
            };
            _deleteAction.Clicked += HandleDeleteClicked;
            ContextActions.Add(_deleteAction);
        }
        #endregion

        #region Finalizers
        ~ContactsListTextCell()
        {
            ContextActions.Remove(_deleteAction);
            _deleteAction.Clicked -= HandleDeleteClicked;
        }
        #endregion

        #region Properties
        ContactsListPage ContactsListPage
        {
            get
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                return navigationPage.Navigation.NavigationStack.FirstOrDefault() as ContactsListPage;

            }
        }

        ContactsListViewModel ContactsListViewModel
        {
            get => ContactsListPage.BindingContext as ContactsListViewModel;
        }

        #endregion

        #region Methods
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Text = string.Empty;
            Detail = string.Empty;

            var item = BindingContext as ContactModel;

            Text = item.FullName;
            Detail = item.PhoneNumber;
        }

        async void HandleDeleteClicked(object sender, EventArgs e)
        {
            var contactSelected = BindingContext as ContactModel;

            await ContactDatabase.DeleteContact(contactSelected);

            ContactsListViewModel.RefreshCommand?.Execute(null);
        }
        #endregion
    }
}
