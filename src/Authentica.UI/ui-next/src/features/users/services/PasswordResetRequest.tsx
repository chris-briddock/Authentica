import { BASE_URL } from "@/common/constants/BaseUrl";
import { IPasswordResetRequest } from "../requests/IPasswordResetRequest";

export const PasswordResetRequest = async (data: IPasswordResetRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/reset-password`;
    const response = await fetch(URL, {
      method: 'POST',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    return response;
  };