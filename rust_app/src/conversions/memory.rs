use std::ffi::CString;
use std::os::raw::c_char;

#[no_mangle]
pub extern "C" fn free_rust_string(s: *mut c_char) {
    if !s.is_null() {
        unsafe {
            // CString を生成してメモリを解放し、戻り値を無視
            let _ = CString::from_raw(s);
        }
    }
}