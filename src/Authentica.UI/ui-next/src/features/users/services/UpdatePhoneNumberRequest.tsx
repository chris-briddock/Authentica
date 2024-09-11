import { BASE_URL } from "@/common/constants/BaseUrl";
import { IUpdatePhoneNumberRequest } from "../requests/IUpdatePhoneNumberRequest";

export const UpdatePhoneNumberRequest = async (data: IUpdatePhoneNumberRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/details/number`;
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