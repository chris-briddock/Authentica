import { BASE_URL } from "@/common/constants/BaseUrl";
import { ICreateApplicationRequest } from "@/features/applications/requests/ICreateApplicationRequest";

export const CreateApplicationRequest = async (data: ICreateApplicationRequest): Promise<Response> => {
    const URL = `${BASE_URL}/applications`;
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