import logo from "@assets/icons/logo.svg";
import logoText from "@assets/icons/logoText.svg";

export const SideBar = () => {
  return (
    <div className="p-6 flex flex-col">
      <div className="flex flex-row items-center gap-x-2 mb-6">
        <img src={logo} alt="" />
        <img src={logoText} alt="" />
      </div>
    </div>
  );
};
