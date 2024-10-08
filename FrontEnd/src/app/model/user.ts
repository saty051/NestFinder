// Defining the User interface to represent user-related information.
export interface UserForRegister {
    userName: string;
    email: string;
    password: string;
    mobile: number;
}


export interface UserForLogin {
    userName: string;
    password: string;
    token: string;
}