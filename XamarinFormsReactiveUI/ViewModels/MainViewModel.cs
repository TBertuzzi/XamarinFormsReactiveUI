using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace XamarinFormsReactiveUI.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        [Reactive] public string Usuario { get; set; }
        [Reactive] public string Senha { get; set; }

        public bool Ocupado { [ObservableAsProperty] get; }

        [Reactive] public ReactiveCommand<Unit, bool> LogarCommand { get; private set; }

        public MainViewModel()
        {
          
                LogarCommand =
                   ReactiveCommand
                       .CreateFromTask(
                           async () =>
                           {
                               return await EfetuarLogin();
                           },
                           this.WhenAnyValue(x => x.Usuario, x => x.Senha,
                               (usuario, senha) =>
                                   !string.IsNullOrWhiteSpace(usuario) &&
                                   !string.IsNullOrWhiteSpace(senha)));


            this.WhenAnyObservable(x => x.LogarCommand.IsExecuting)
                .StartWith(false)
                .ToPropertyEx(this, x => x.Ocupado);


        }

        private async Task<bool> EfetuarLogin()
        {
            await CurrentApplication
                     .MainPage.DisplayAlert("Login Efetuado", "Sucesso", "OK");

            return true;
        }
    }
}
