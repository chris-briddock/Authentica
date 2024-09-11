import { BASE_URL } from "@/common/constants/BaseUrl";
import { IUpdateAddressRequest } from "../requests/IUpdateAddressRequest";

export const UpdateAddressRequest = async (data: IUpdateAddressRequest): Promise<Response> => {
    const URL = `${BASE_URL}/users/details/address`;
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