import { BASE_URL } from "@/common/constants/BaseUrl";
import { IDeleteApplicationRequest } from "../requests/IDeleteApplicationRequest";


export const DeleteApplicationRequest = async (data: IDeleteApplicationRequest): Promise<Response> => {
    const URL = `${BASE_URL}/applications`;
    const response = await fetch(URL, {
      method: 'DELETE',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    return response;
  };