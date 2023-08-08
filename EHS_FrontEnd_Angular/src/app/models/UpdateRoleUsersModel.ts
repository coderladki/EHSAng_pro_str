import { UserRoleModel } from "./UserRoleModel";

export interface UpdateRoleUsersModel {
    roleId: number
    userRoleModelList: UserRoleModel[]
}