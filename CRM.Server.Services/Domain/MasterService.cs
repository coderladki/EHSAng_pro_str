using System.Threading.Tasks;
using System.Threading;
using EHS.Server.Models.Masters;
using EHS.Server.Data;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.Excel;
using CRM.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System;
using DocumentFormat.OpenXml.Bibliography;
using Dapper;

namespace EHS.Server.Services.Domain
{
    public class MasterService
    {

        private readonly MasterRepository _masterRepository;

        public MasterService(string connectionString)
        {
            _masterRepository = new MasterRepository(connectionString);
        }
        public async Task<Division> CreateDivisionAsync(Division division, CancellationToken cancellationToken)
        {
            return await _masterRepository.CreateDivisionAsync(division, cancellationToken).ConfigureAwait(false);
        }
        public async Task<Division> FindDivisionByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _masterRepository.FindDivisionByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }
        public async Task<List<Division>> GetAllDivisionAsync()
        {
            return await _masterRepository.GetAllDivisionAsync().ConfigureAwait(false);
        }

        public async Task<Division> UpdateDivisionAsync(Division division, CancellationToken cancellationToken)
        {
            return await _masterRepository.UpdateDivisionAsync(division, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Division> DeleteDivisionAsync(int id)
        {
            return await _masterRepository.DeleteDivisionAsync(id).ConfigureAwait(false);
        }

        
        #region Email Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To Add update  email master 
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailsMaster> AddUpdateEmailMasterAsync(EmailsMaster emailsMaster, CancellationToken cancellationToken)
        {
            return await _masterRepository.AddUpdateEmailMasterAsync(emailsMaster, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To get  email master
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <returns></returns>
        public async Task<List<EmailsMaster>> GetAllEmailMasterAsync(EmailsMaster emailsMaster)
        {
            return await _masterRepository.GetAllEmailMasterAsync(emailsMaster);
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 27/02/2023
        /// To delete  email master
        /// </summary>
        /// <param name="emailsMaster"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailsMaster> DeleteEmailMasterAsync(EmailsMaster emailsMaster)
        {
            return await _masterRepository.DeleteEmailMasterAsync(emailsMaster).ConfigureAwait(false);
        }
        #endregion


        #region Employee Master

        public async Task<Employee> CreateEmployeeAsync(Employee employee, UserManager<ApplicationUser> _userManager, CancellationToken cancellationToken)
        {
            return await _masterRepository.CreateEmployeeAsync(employee,_userManager, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            return await _masterRepository.GetAllEmployeeAsync().ConfigureAwait(false);
        }

        public async Task<Employee> FindEmployeeByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _masterRepository.FindEmployeeByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            return await _masterRepository.UpdateEmployeeAsync(employee, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            return await _masterRepository.DeleteEmployeeAsync(id).ConfigureAwait(false);
        }
        
        #endregion

        #region Unit Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To  create plant/unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="_userManager"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> CreateUnitAsync(Unit unit, UserManager<ApplicationUser> _userManager, CancellationToken cancellationToken)
        {
            return await _masterRepository.CreateUnitAsync(unit, _userManager, cancellationToken).ConfigureAwait(false);
        }        
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To get  plant/unit List
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public async Task<List<UnitDetails>> GetAllUnitAsync(Unit unit)
        {
            return await _masterRepository.GetAllUnitAsync(unit);
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 02/03/2023
        /// To delete  unit/plant master
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> DeleteUnitAsync(Unit unit)
        {
            return await _masterRepository.DeleteUnitAsync(unit).ConfigureAwait(false);
        }
        #endregion
        #region Modules
        public async Task<List<Modules>> GetAllModulesAsync()
        {
            return await _masterRepository.GetAllModulesAsync().ConfigureAwait(false);
        }
        #endregion

        #region Department Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update department
        /// </summary>
        /// <param name="department"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Models.Masters.Department> AddUpdateDepartmentAsync(Models.Masters.Department department, CancellationToken cancellationToken)
        {
            try
            {                   
                return await _masterRepository.AddUpdateDepartmentAsync(department,cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all departments
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public async Task<List<Models.Masters.DepartmentDetails>> GetAllDepartmentsAsync(Models.Masters.Department department)
        {
            try
            {
                return await _masterRepository.GetAllDepartmentsAsync(department);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  department
        /// </summary>
        /// <param name="department"></param>        
        /// <returns></returns>
        public async Task<Models.Masters.Department> DeleteDepartmentAsync(Models.Masters.Department department)
        {
            try
            {
                return await _masterRepository.DeleteDepartmentAsync(department);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Department Section Master
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update department Section
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Sections> AddUpdateDepartmentSectionAsync(Sections sections, CancellationToken cancellationToken)
        {
            try
            {
                return await _masterRepository.AddUpdateDepartmentSectionAsync(sections,cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all sections
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public async Task<List<DepartmentSectionDetails>> GetAllDepartmentSectionAsync(Sections sections)
        {
            try
            {
                return await _masterRepository.GetAllDepartmentSectionAsync(sections);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  department section
        /// </summary>
        /// <param name="sections"></param>        
        /// <returns></returns>
        public async Task<Sections> DeleteDepartmentSectionAsync(Sections sections)
        {
            try
            {
                return await _masterRepository.DeleteDepartmentSectionAsync(sections);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Responsible persons Department
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To Add or update Responsible Person
        /// </summary>
        /// <param name="person"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponsiblePerson> AddUpdateResponsiblePersonAsync(ResponsiblePerson person, CancellationToken cancellationToken)
        {
            try
            {
                return await _masterRepository.AddUpdateResponsiblePersonAsync(person,cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To get all person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<List<ResponsiblePersonDetails>> GetAllResponsiblePersonAsync(ResponsiblePerson person)
        {
            try
            {
                return await _masterRepository.GetAllResponsiblePersonAsync(person);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Author: Ajay Singh
        /// Created Date: 06/03/2023
        /// To delete  person
        /// </summary>
        /// <param name="person"></param>        
        /// <returns></returns>
        public async Task<ResponsiblePerson> DeleteResponsiblePersonAsync(ResponsiblePerson person)
        {
            try
            {
                return await _masterRepository.DeleteResponsiblePersonAsync(person);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region
        /// <summary>
        /// Author: Preeti Juyal
        /// Created Date: 08/03/2023
        /// To get all PPPETypes
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<List<PPPEType>> GetAllPPPETypesAsync(PPPEType pppeTypeId)
        {
            try
            {
                return await _masterRepository.GetAllPPPETypesAsync(pppeTypeId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Author: Preeti Juyal
        /// Created Date : 08/03/2023
        /// Get default precautions master if precautionId is not provided else the Default Precaution with given Id
        /// </summary>
        /// <param name="precautionId"></param>
        /// <returns></returns>
        public async Task<List<Precaution>> GetAllDefaultPrecautionsAsync(int precautionId)
        {
            try
            {
                return await _masterRepository.GetAllDefaultPrecautionsAsync(precautionId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
