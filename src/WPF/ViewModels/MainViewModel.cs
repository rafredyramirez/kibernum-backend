using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using WPF.Helpers;

namespace WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAreaService _areaService;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Campos
        private string _name;
        private string _contactInfo;
        private int _roleId;
        private int _areaId;
        private int _selectedUserId;
        private bool _isAreaEnabled = false;
        private UserGridDto _selectedUser;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string ContactInfo
        {
            get => _contactInfo;
            set { _contactInfo = value; OnPropertyChanged(nameof(ContactInfo)); }
        }

        public int RoleId
        {
            get => _roleId;
            set { _roleId = value; OnPropertyChanged(nameof(RoleId)); }
        }

        public int AreaId
        {
            get => _areaId;
            set { _areaId = value; OnPropertyChanged(nameof(AreaId)); }
        }

        public int SelectedUserId
        {
            get => _selectedUserId;
            set { _selectedUserId = value; OnPropertyChanged(nameof(SelectedUserId)); }
        }

        public UserGridDto SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;

                if (_selectedUser != null)
                {
                    SelectedUserId = _selectedUser.Id;
                    Name = _selectedUser.Name;
                    ContactInfo = _selectedUser.ContactInfo;

                    var role = Roles.FirstOrDefault(r => r.Name.Trim().ToLower() == _selectedUser.Name.Trim().ToLower());
                    if (role != null)
                        RoleId = role.Id;

                    RoleId = _selectedUser.RoleId;

                    AreaId = _selectedUser.AreaId ?? 0;

                    if (_selectedUser.Name != "Sin asignar")
                    {
                        var area = Areas.FirstOrDefault(a => a.Name == _selectedUser.Name);
                        if (area != null)
                            AreaId = area.Id;
                    }
                    else
                    {
                        AreaId = 0;
                    }

                    IsAreaEnabled = true;
                }

                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public bool IsAreaEnabled
        {
            get => _isAreaEnabled;
            set
            {
                _isAreaEnabled = value;
                OnPropertyChanged(nameof(IsAreaEnabled));
            }
        }

        // Colecciones
        public ObservableCollection<UserGridDto> Users { get; set; } = new();
        public ObservableCollection<Role> Roles { get; set; } = new();
        public ObservableCollection<Area> Areas { get; set; } = new();

        // Commands
        public ICommand CreateUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand AssignAreaCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ClearCommand { get; }

        public MainViewModel(IUserService userService, IRoleService roleService, IAreaService areaService)
        {
            _userService = userService;
            _roleService = roleService;
            _areaService = areaService;

            CreateUserCommand = new RelayCommand(async _ => await CreateUser());
            UpdateUserCommand = new RelayCommand(async _ => await UpdateUser());
            AssignAreaCommand = new RelayCommand(async _ => await AssignArea());
            DeleteUserCommand = new RelayCommand(async _ => await DeleteUser());
            ClearCommand = new RelayCommand(_ =>
            {
                ClearForm();
                return Task.CompletedTask;
            });
            IsAreaEnabled = false;
            _ = Initialize();
        }

        public async Task LoadUsers()
        {
            var list = await _userService.GetLastUsersAsync();

            Users.Clear();
            foreach (var user in list)
                Users.Add(user);
        }

        public async Task LoadCatalogs()
        {
            var roles = await _roleService.GetRolesAsync();
            Roles.Clear();
            foreach (var r in roles)
                Roles.Add(r);
            var areas = await _areaService.GetAreasAsync();
            Areas.Clear();
            foreach (var a in areas)
                Areas.Add(a);
        }

        private async Task Initialize()
        {
            await LoadCatalogs();
            await LoadUsers();
        }

        private async Task CreateUser()
        {
            try
            {
                if (SelectedUserId > 0)
                {
                    MessageBox.Show("No puedes crear un usuario existente.");
                    return;
                }

                await _userService.CreateUserAsync(Name, ContactInfo, RoleId);

                MessageBox.Show("Usuario creado correctamente");

                await LoadUsers();

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task UpdateUser()
        {
            try
            {
                await _userService.UpdateUserAsync(SelectedUserId, Name, ContactInfo, RoleId);
                MessageBox.Show("Usuario actualizado");

                await LoadUsers();
                ClearForm(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task AssignArea()
        {
            try
            {
                await _userService.AssignAreaAsync(SelectedUserId, AreaId);
                MessageBox.Show("Área asignada correctamente");
                await LoadUsers(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task DeleteUser()
        {
            try
            {
                if (SelectedUserId == 0)
                {
                    MessageBox.Show("Seleccione un usuario");
                    return;
                }

                var result = MessageBox.Show(
                    "¿Está seguro de eliminar el usuario?",
                    "Confirmación",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                    return;

                await _userService.DeleteUserAsync(SelectedUserId);

                MessageBox.Show("Usuario eliminado");

                await LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearForm()
        {
            Name = "";
            ContactInfo = "";
            RoleId = 0;
            AreaId = 0;
            SelectedUserId = 0;
            SelectedUser = null;
            IsAreaEnabled = false;
        }
    }
}
