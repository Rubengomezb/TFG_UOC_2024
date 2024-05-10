using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using Syncfusion.Maui.Picker;
using Syncfusion.Maui.Scheduler;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP.Behaviours
{
    public class MenuBehaviour : Behavior<ContentPage>
    {
        /// <summary>
        /// The no events label.
        /// </summary>
        private Label? noEventsLabel;

        /// <summary>
        /// The appointment list view
        /// </summary>
        private ListView? appointmentListView;

        /// <summary>
        /// schedule initialize
        /// </summary>
        private SfScheduler? scheduler;

 
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            this.scheduler = bindable.FindByName<SfScheduler>("Scheduler");
            this.noEventsLabel = bindable.FindByName<Label>("noEventsLabel");
            this.appointmentListView = bindable.FindByName<ListView>("appointmentListView");
            if (scheduler != null)
            {
                scheduler.ViewChanged += this.OnSchedulerViewChanged;
                scheduler.Tapped += Scheduler_Tapped;
                scheduler.SelectionChanged += Scheduler_SelectionChanged;
                scheduler.QueryAppointments += Scheduler_QueryAppointments;
            }


        }

        private async void Scheduler_QueryAppointments(object sender, SchedulerQueryAppointmentsEventArgs e)
        {
            var viewModel = this.scheduler.BindingContext as MenuViewModel;
            viewModel.QueryAppointments(e.VisibleDates.ToList());
        }

        private async void Scheduler_SelectionChanged(object? sender, SchedulerSelectionChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                //// Listview takes time to update items source hence uses delay so that scheduler will be loaded and after the delay listview items will be generated.
                await Task.Delay(50);
                this.UpdateMonthAgendaViewDetails(e.NewValue.Value);
            }
        }

        private void Scheduler_Tapped(object? sender, SchedulerTappedEventArgs e)
        {
            if (e.Element != SchedulerElement.SchedulerCell)
            {
                return;
            }

            if (scheduler != null && e.Date != null && e.Date != scheduler.SelectedDate)
            {
                this.UpdateMonthAgendaViewDetails(e.Date.Value);
            }
        }

        private void UpdateMonthAgendaViewDetails(DateTime? tappedDate)
        {
            if (this.scheduler == null || this.noEventsLabel == null || this.appointmentListView == null || tappedDate == null)
            {
                return;
            }

            var viewModel = this.scheduler.BindingContext as MenuViewModel;
            if (viewModel == null || tappedDate == viewModel.SelectedDate)
            {
                return;
            }

            if (tappedDate.Value.Date == DateTime.Now.Date)
            {
                viewModel.IsToday = true;
                viewModel.DateTextColor = Colors.White;
            }
            else
            {
                viewModel.IsToday = false;
                viewModel.DateTextColor = Colors.Black;
            }

            if (tappedDate != viewModel.SelectedDate)
            {
                viewModel.SelectedDate = tappedDate.Value.Date;
            }

            var appointments = viewModel.GetSelectedDateAppointments(tappedDate.Value.Date);

            if (appointments != null && appointments.Any())
            {
                viewModel.SelectedDateMenus = appointments.Where(x => !viewModel.SelectedDateMenus.Any(y => y.Id == x.Id && y.Name == x.Name)).ToObservableCollection();
                this.appointmentListView.IsVisible = true;
                this.noEventsLabel.IsVisible = false;
            }
            else
            {
                this.appointmentListView.IsVisible = false;
                this.noEventsLabel.IsVisible = true;
            }
        }

        private void OnSchedulerViewChanged(object? sender, SchedulerViewChangedEventArgs e)
        {
            if (e.NewView != SchedulerView.Month && this.scheduler != null)
            {
                this.scheduler.View = SchedulerView.Month;
                return;
            }

            if (this.scheduler != null && this.scheduler.SelectedDate != null && e.NewVisibleDates != null && e.NewVisibleDates.Count > 0)
            {
                scheduler.SelectedDate = e.NewVisibleDates[0];
            }
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);

            if (this.scheduler != null)
            {
                scheduler.ViewChanged -= this.OnSchedulerViewChanged;
                scheduler.Tapped -= this.Scheduler_Tapped;
                scheduler.SelectionChanged -= this.Scheduler_SelectionChanged;
                this.scheduler = null;
            }

            if (this.noEventsLabel != null)
            {
                this.noEventsLabel = null;
            }

            if (this.appointmentListView != null)
            {
                this.appointmentListView = null;
            }
        }
    }
}
