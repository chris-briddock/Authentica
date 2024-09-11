import { IFormState } from "@/common/models/IFormState";
import { useState } from "react";
  
export const useForm = <T extends IFormState>(initialState: T) => {
    const [formData, setFormData] = useState<T>(initialState);
  
    const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, value } = e.target;
      setFormData(prevState => ({
        ...prevState,
        [name]: value,
      }));
    };
  
    return { formData, handleFormChange };
  };