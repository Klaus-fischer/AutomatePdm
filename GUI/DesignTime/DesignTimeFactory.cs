namespace AutomatePdm.GUI.DesignTime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class DesignTimeFactory
    {
        private static MainViewModel? mainViewModel;
        private static object? popupHostViewModel;

        public static MainViewModel MainViewModel => mainViewModel ??= CreateMainViewModel();

        public static object PopupHostViewModel => popupHostViewModel ??= CreatePopupHostViewModel();

        private static MainViewModel CreateMainViewModel()
        {
            var vm = new MainViewModel();
            vm.Triggers.Add(new TriggerViewModel()
            {
                Title = "Statusänderung nach Checkin",
                Description = "Ändert den status von Irgendwas zu Irgendwas anderes.",
                TriggerName = nameof(PostCheckinTrigger),
                EditTriggerCommand = vm.EditTriggerCommand,
            });

            vm.Triggers.Add(new TriggerViewModel()
            {
                Title = "Statusänderung nach Checkout",
                Description = "Ändert den status von Irgendwas zu Irgendwas anderes.",
                TriggerName = nameof(PostCheckoutTrigger),
                EditTriggerCommand = vm.EditTriggerCommand,
            });

            vm.Triggers.Add(new TriggerViewModel()
            {
                Title = "Prüfe Bedingung vor Statuswechsel",
                Description = "Eigenschaft XY muss gesetzt sein, sonst kein Statuswechsel.",
                TriggerName = nameof(PreviewStateChangeTrigger),
                EditTriggerCommand = vm.EditTriggerCommand,
            });

            vm.Triggers.Last().Script.Add(new ProcessStepViewModel()
            {
                Title = "Dateityp ist SldWorks",
                ProcessStepName = nameof(ConditionFilter),
            });

            vm.Triggers.Last().Script.Add(new ProcessStepViewModel()
            {
                Title = "Status ist 'in Bearbeitung'",
                ProcessStepName = nameof(ConditionFilter),
            });

            vm.Triggers.Last().Script.Add(new ProcessStepViewModel()
            {
                Title = "Zielstatus ist 'Freigegeben'",
                ProcessStepName = nameof(ConditionFilter),
            });

            vm.Triggers.Last().Script.Add(new ProcessStepViewModel()
            {
                Title = "Eigenschaft XY ist nicht ''",
                ProcessStepName = nameof(ConditionFilter),
            });

            vm.Triggers.SelectedItem = vm.Triggers.Last();
            vm.PopupTitle = "Eigenschaften Bearbeiten";

            return vm;
        }

        private static object CreatePopupHostViewModel()
        {
            return new TriggerViewModel()
            {
                Title = "Statusänderung nach Checkin",
                Description = "Ändert den status von Irgendwas zu Irgendwas anderes.",
                TriggerName = nameof(PostCheckinTrigger),
                EditTriggerCommand = null!,
            };
        }
    }
}
