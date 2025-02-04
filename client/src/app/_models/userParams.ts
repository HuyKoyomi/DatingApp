import { User } from './user';

export class UserParams {
  pageNumber = 1;
  pageSize = 5;
  minAge = 18;
  maxAge = 100;
  gender: string;
  orderBy= 'lastActive';

  constructor(user: User | null) {
    this.gender = user?.gender === 'male' ? 'female' : 'male';
  }
}

/*
    Sự khác nhau giữa class. interface, type
    1. Class
        + Mục đích: Dùng để tạo ra các đối tượng (instances) với các thuộc tính (properties) và phương thức (methods).
        + Tính năng:
            - Có thể khởi tạo đối tượng bằng từ khóa new.
            - Hỗ trợ kế thừa (inheritance) bằng từ khóa extends.
            - Có thể triển khai các phương thức và logic bên trong.
            - Có thể sử dụng các modifier như public, private, protected.
        + class Animal {
                name: string;
                constructor(name: string) {
                    this.name = name;
                }
                speak(): void {
                    console.log(`${this.name} makes a noise.`);
                }
            }

        const dog = new Animal("Dog");
        dog.speak(); // Output: Dog makes a noise.
    
    2. Interface
        + Mục đích: Dùng để định nghĩa cấu trúc (shape) của một đối tượng hoặc hợp đồng (contract) mà các lớp hoặc đối tượng phải tuân theo.
        + Tính năng:
            - Không thể khởi tạo đối tượng trực tiếp từ interface.
            - Chỉ định nghĩa các thuộc tính và phương thức mà không có triển khai cụ thể.
            - Hỗ trợ kế thừa (extends) giữa các interface.
            - Có thể được sử dụng để mô tả kiểu dữ liệu cho các đối tượng, hàm, lớp, v.v.
    3. Type
        + Mục đích: Dùng để định nghĩa kiểu dữ liệu tùy chỉnh (custom types) hoặc kết hợp các kiểu dữ liệu khác nhau.
        + Tính năng:
            - Không thể khởi tạo đối tượng trực tiếp từ type.
            - Có thể kết hợp các kiểu dữ liệu bằng các toán tử như | (union), & (intersection).
            - Có thể định nghĩa kiểu dữ liệu cho các biến, hàm, đối tượng, v.v.
            - Không hỗ trợ kế thừa như interface, nhưng có thể mở rộng bằng cách sử dụng &.
    
    4. Khi nào sử dụng:
        - Class: Khi cần tạo đối tượng với logic và hành vi cụ thể.
        - Interface: Khi cần định nghĩa cấu trúc dữ liệu hoặc hợp đồng mà các lớp/đối tượng phải tuân theo.
        - Type: Khi cần định nghĩa kiểu dữ liệu phức tạp hoặc kết hợp nhiều kiểu dữ liệu.
    
        // Union và Intersection là hai khái niệm quan trọng giúp kết hợp các kiểu dữ liệu.

        type StringOrNumber = string | number; // Union
        type EmployeeInfo = Person & Employee; // Intersection (type Person + type Employee)
        
*/
