import { BASE_URL } from "@/common/constants/BaseUrl";
import { IUpdateEmailRequest } from "../requests/IUpdateEmailRequest";

export const UpdateEmailRequest = async (data: IUpdateEmailRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/details/email`;
    const response = await fetch(URL, {
      method: 'PUT',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    return response;
  };