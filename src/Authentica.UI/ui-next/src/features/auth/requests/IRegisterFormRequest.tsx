import { IFormState } from "../../../common/models/IFormState";

export interface IRegisterFormRequest extends IFormState {
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
}