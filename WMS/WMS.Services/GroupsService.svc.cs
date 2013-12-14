using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.DatabaseAccess.Entities;

namespace WMS.Services
{
    /// <summary>
    /// Servis obsługujący zapytania związane z partiami i przesunięciami
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class GroupsService : ServiceBase, IGroupsService
    {
        /// <summary>
        /// Pobiera partie, znajdujące się aktualnie w zadanym sektorze.
        /// </summary>
        /// <param name="sectorId">Zapytanie z id sektora dla którego pobieramy znajdujące się w nim partie</param>
        /// <returns>Odpowiedź z listą partii</returns>
        public Response<List<ShiftDto>> GetSectorGroups(Request<int> sectorId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(sectorId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest && x.Group.SectorId == sectorId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacja o zadanej grupie
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        public Response<GroupDto> GetGroupInfo(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<GroupDto>(groupId.Id, Transaction(tc =>
                tc.Entities.Groups.Where(x => x.Id == groupId.Content).
                    Include(x => x.Sector.Warehouse).
                Select(groupAssembler.ToGroupDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera historię grupy jako listę przesunięć, jakie były wykonywane na tej grupie.
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        public Response<List<ShiftDto>> GetGroupHistory(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(groupId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.GroupId == groupId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera wszystkie przesunięcie, któr są ostatnimi dla swoich partii.
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą przesunięć</returns>
        public Response<List<ShiftDto>> GetShifts(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(request.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacje o produktach znajdujących się w partii.
        /// </summary>
        /// <param name="groupId">Zapytanie z id partii</param>
        /// <returns>Odpowiedź z listą szczegułów produktów przesyłanych w partii wraz z ich lościami</returns>
        public Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ProductDetailsDto>>(groupId.Id, Transaction(tc =>
                tc.Entities.GroupsDetails.Where(x => x.GroupId == groupId.Content).
                    Include(x => x.Product).
                Select(productAssembler.ToDetailsDto).ToList()));
        }

        /// <summary>
        /// Dodaje nowe przesunięcie i przesuwa partię aktualizując jej położenie
        /// i ustawiając ostatnie przesunięcie danej partii na bierzące.
        /// Jeśli jest to próba przesunięcia partii, która już została wydana
        /// do magazynu zewnętrznego (partnera), to rzucany jest wyjątek.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem, które ma być dodane</param>
        /// <returns>Odpowiedź z wykonanym przesunięciem</returns>
        public Response<ShiftDto> AddNew(Request<ShiftDto> shift)
        {
            // TODO - uaktualnić położenie partii i ustawić poprawnie latest na przesunięciach
            // no i ewentualnie psrawdzać czy nie wykonuje się złe przesunięcie
            CheckPermissions(PermissionLevel.User);
            Shift s = null;
            Transaction(tc => s = tc.Entities.Shifts.Add(groupAssembler.ToEntity(shift.Content)));
            return new Response<ShiftDto>(shift.Id, groupAssembler.ToShiftDto(s));
        }

        /// <summary>
        /// Dodaje nową grupę wraz z pierwszym przesunięciem. Ilości produktów w tworzonej partii muszą być nieujemne,
        /// w przeciwnym przypadku rzucany jest wyjątek.
        /// </summary>
        /// <param name="group">Zapytanie z dodawaną grupą</param>
        /// <returns>Odpowiedź z dodaną grupą</returns>
        public Response<GroupDto> AddNew(Request<GroupDto> group)
        {
            // TODO - sprawdzać ilości produktów i je w ogłe dodawać (w groupAssembler)
            CheckPermissions(PermissionLevel.User);
            Group g = null;
            Transaction(tc => g = tc.Entities.Groups.Add(groupAssembler.ToEntity(group.Content)));
            return new Response<GroupDto>(group.Id, groupAssembler.ToGroupDto(g));
        }

        /// <summary>
        /// Sprawdzenie, czy nadawca przesunięcia jest magazynem wewnętrznym.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem do sprawdzenia</param>
        /// <returns>Odpowiedź true albo false</returns>
        public Response<bool> IsSenderInternal(Request<ShiftDto> shift)
        {
            CheckPermissions(PermissionLevel.User);
            bool ret = false;
            Transaction(tc =>
                {
                    var s = tc.Entities.Groups.Include(x => x.Shifts).Where(x => x.Id == shift.Content.Id).FirstOrDefault().Shifts.Where(x => x.Latest == true).FirstOrDefault();
                    ret = tc.Entities.Warehouses.Find(s.SenderId).Internal;
                });
            return new Response<bool>(shift.Id, ret);
        }
    }
}
