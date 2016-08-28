using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using PresentationDemos.Models;
using PresentationDemos.Services;
using ReactiveUI;
using ReactiveUI.Routing;
using Splat;

namespace PresentationDemos.ViewModels
{
    public class TodoListViewModel : ReActivatableObject<Unit, TodoListViewModel.State>
    {
        private ReactiveList<Todo> todos = new ReactiveList<Todo>();
        private int maxTodos;
        private string newTodo = "";

        public class State
        {
            public Todo[] Todos { get; set; }
        }

        public ReactiveList<Todo> Todos
        {
            get { return todos; }
            set { this.RaiseAndSetIfChanged(ref todos, value); }
        }

        public string NewTodo
        {
            get { return newTodo; }
            set { this.RaiseAndSetIfChanged(ref newTodo, value); }
        }

        public int MaxTodos
        {
            get { return maxTodos; }
            private set { this.RaiseAndSetIfChanged(ref maxTodos, value); }
        }

        public ISettingsService Settings { get; }
        public IRouter Router { get; }

        public ReactiveCommand<Unit> ToggleTodo { get; }
        public ReactiveCommand<Unit> DeleteTodo { get; }
        public ReactiveCommand<Unit> CreateTodo { get; }
        public ReactiveCommand<Unit> Load { get; }
        public ReactiveCommand<Unit> ViewSettings { get; }

        public TodoListViewModel(IRouter router = null, ISettingsService settings = null)
        {
            Router = router ?? Locator.Current.GetService<IRouter>();
            Settings = settings ?? Locator.Current.GetService<ISettingsService>();
            Load = ReactiveCommand.CreateAsyncTask(o => LoadImpl());
            var canCreateTodo = this.WhenAny(
                vm => vm.NewTodo,
                vm => vm.MaxTodos,
                vm => vm.Todos.Count,
                (newTodo, max, todosCount) => !string.IsNullOrEmpty(newTodo.Value) && todosCount.Value < max.Value);
            CreateTodo = ReactiveCommand.CreateAsyncTask(canCreateTodo, o => CreateTodoImpl());
            DeleteTodo = ReactiveCommand.CreateAsyncTask(o => DeleteTodoImpl((int)o));
            ToggleTodo = ReactiveCommand.CreateAsyncTask(o => ToggleTodoImpl((int)o));
            ViewSettings = ReactiveCommand.CreateAsyncTask(o => ViewSettingsImpl());
        }

        private async Task ViewSettingsImpl()
        {
            await Router.ShowAsync<SettingsViewModel>();
        }

        private async Task LoadImpl()
        {
            MaxTodos = await Settings.GetMaxTodos();
        }

        private Task ToggleTodoImpl(int index)
        {
            var todo = Todos[index];
            todo.Completed = !todo.Completed;
            return Task.FromResult(Unit.Default);
        }

        private Task DeleteTodoImpl(int index)
        {
            Todos.RemoveAt(index);
            return Task.FromResult(Unit.Default);
        }

        private Task CreateTodoImpl()
        {
            var todo = new Todo()
            {
                Name = NewTodo,
                Completed = false
            };
            Todos.Add(todo);
            NewTodo = "";
            return Task.FromResult(Unit.Default);
        }

        protected override void ResumeCoreSync(State storedState)
        {
            base.ResumeCoreSync(storedState);
            Todos = new ReactiveList<Todo>(storedState.Todos);
        }

        protected override State GetStateCoreSync()
        {
            var state = base.GetStateCoreSync();
            state.Todos = Todos.ToArray();
            return state;
        }
    }
}
