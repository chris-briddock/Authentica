import { BASE_URL } from "@/common/constants/BaseUrl";
import { IUpdateApplicationRequest } from "../requests/IUpdateApplicationRequest";

export const UpdateApplicationRequest = async (data: IUpdateApplicationRequest): Promise<Response> => {
    const URL = `${BASE_URL}/applications`;
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